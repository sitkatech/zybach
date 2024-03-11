import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorStatusService } from 'src/app/shared/generated/api/sensor-status.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { SensorSimpleDto } from 'src/app/shared/generated/model/sensor-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellWithSensorSimpleDto } from 'src/app/shared/generated/model/well-with-sensor-simple-dto';
import { WellMapComponent } from '../well-map/well-map.component';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { FieldDefinitionTypeEnum } from 'src/app/shared/generated/enum/field-definition-type-enum';
import { ContinuityMeterStatusEnum } from 'src/app/shared/generated/enum/continuity-meter-status-enum';
import { SensorTypeEnum } from 'src/app/shared/generated/enum/sensor-type-enum';
import { ColDef } from 'ag-grid-community';
import { DecimalPipe } from '@angular/common';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';

@Component({
  selector: 'zybach-sensor-status',
  templateUrl: './sensor-status.component.html',
  styleUrls: ['./sensor-status.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SensorStatusComponent implements OnInit, OnDestroy {
  @ViewChild("wellsGrid") wellsGrid: any;
  @ViewChild("wellMap") wellMap: WellMapComponent;

  public watchUserChangeSubscription: Subscription;
  public currentUser: UserDto;

  public wellsGeoJson: object;
  public redSensors: SensorSimpleDto[];
  public columnDefs: ColDef[];
  public defaultColDef: ColDef;

  public richTextTypeID = CustomRichTextTypeEnum.SensorStatusMap;

  private static messageAgeMax = 1440; // 24 hours in minutes
  private static voltageReadingMin = 2500;

  constructor(
    private authenticationService: AuthenticationService,
    private sensorStatusService: SensorStatusService,
    private utilityFunctionsService: UtilityFunctionsService,
    private decimalPipe: DecimalPipe
  ) { }

  ngOnInit(): void {
    this.createColumnDefs();

    this.watchUserChangeSubscription = this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;

      this.sensorStatusService.sensorStatusGet().subscribe(wells => {
        const wellsWithActiveSensors = wells.filter(x => x.Sensors.filter(y => y.IsActive).length > 0);
        this.wellsGeoJson =
        {
          type: "FeatureCollection",
          features:
            wellsWithActiveSensors.map(x => {
              const geoJsonPoint = { 
                type: "Feature", 
                geometry: { 
                  type: "Point", 
                  coordinates: [x.Longitude, x.Latitude]
                },
                properties: {
                  wellID: x.WellID,
                  wellRegistrationID: x.WellRegistrationID,
                  sensors: x.Sensors.filter(y => y.IsActive) || [],
                  AgHubRegisteredUser: x.AgHubRegisteredUser,
                  fieldName: x.FieldName
                }
              };
              return geoJsonPoint;
            })
        };

        this.redSensors = wells
        .reduce((sensors: SensorSimpleDto[], well: WellWithSensorSimpleDto) => 
          sensors.concat(well.Sensors.map(sensor => ({
            ...sensor, 
            WellID: well.WellID, 
            WellRegistrationID: well.WellRegistrationID, 
            AgHubRegisteredUser: well.AgHubRegisteredUser, 
            fieldName: well.FieldName,
            Latitude: well.Latitude,
            Longitude: well.Longitude
          }))
        ), [])
        .filter(sensor => sensor.IsActive && (sensor.MostRecentSupportTicketID != null ||
          sensor.LastMessageAgeInHours > SensorStatusComponent.messageAgeMax || 
          (sensor.LastVoltageReading != null && sensor.LastVoltageReading < SensorStatusComponent.voltageReadingMin) || 
          (sensor.ContinuityMeterStatusID != null && !sensor.SnoozeStartDate && sensor.ContinuityMeterStatusID != ContinuityMeterStatusEnum.ReportingNormally)
        ))
        .sort((a, b) => a.LastReadingDate == b.LastReadingDate ? 0 : a.LastReadingDate > b.LastReadingDate ? 1 : -1);
      });
    });
  }

  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
  }

  private createColumnDefs() {
    const _decimalPipe = this.decimalPipe;
    this.defaultColDef = { filter: true, sortable: true, resizable: true, width: 125 };

    this.columnDefs = [
      {
        headerName: '', valueGetter: function (params: any) {
          return { LinkValue: params.data.WellID, LinkDisplay: "View Well", CssClasses: "btn-md btn-zybach p-1" };
        }, cellRenderer: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/wells/" },
        comparator: this.utilityFunctionsService.linkRendererComparator,
        sortable: false, filter: false, width: 100
      },
      {
        valueGetter: params => {
          return { LinkValue: `${params.data.SensorID}/new-support-ticket`, LinkDisplay: "Create Ticket", CssClasses: "btn-sm btn-zybach p-1" };
        },
        cellRenderer: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/sensors" },
        sortable: false, filter: false, width: 130
      },
      { 
        field: 'WellRegistrationID', 
        headerComponent: FieldDefinitionGridHeaderComponent, 
        headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellRegistrationNumber, labelOverride: 'Well Number' },
      },
      {
        field: "AgHubRegisteredUser",
        headerComponent: FieldDefinitionGridHeaderComponent, 
        headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.AgHubRegisteredUser, labelOverride: 'AgHub User' },
        
      },
      {
        headerName: "Field Name",
        field: "fieldName",
        headerComponent: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellFieldName},
      },
      {
        headerName: 'Sensor', valueGetter: function (params: any) {
          if(params.data.SensorName) {
            return { LinkValue: params.data.SensorID, LinkDisplay: params.data.SensorName };
          } else {
            return { LinkValue: null, LinkDisplay: null };
          }
        }, 
        cellRenderer: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/sensors/" },
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: function (params: any) {
          return params.data.SensorName;
        },
      },
      {
        headerName:"Active Support Ticket",
        headerComponent: FieldDefinitionGridHeaderComponent, 
        headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.ActiveSupportTicket },
        valueGetter: params => {
          return { 
            LinkValue: params.data.MostRecentSupportTicketID, 
            LinkDisplay: params.data.MostRecentSupportTicketID ? `#${params.data.MostRecentSupportTicketID}: ${params.data.MostRecentSupportTicketTitle}` : ''
          }
        },
        cellRenderer: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/support-tickets/"},
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: params => params.data.MostRecentSupportTicketID ? `#${params.data.MostRecentSupportTicketID}: ${params.data.MostRecentSupportTicketTitle}` : ''
      },
      { headerName: 'Last Message Age (Hours)',
        valueGetter: (params) => params.data.LastMessageAgeInHours ? params.data.LastMessageAgeInHours : null,
        valueFormatter: params => params.value != null ? _decimalPipe.transform(params.value, '1.0-0') : '-',
        filter: 'agNumberColumnFilter', cellStyle: { textAlign: 'right' },
        headerComponent: FieldDefinitionGridHeaderComponent, headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.SensorLastMessageAgeHours },
      },
      this.utilityFunctionsService.createDateColumnDef('Last Measurement Date', 'LastReadingDate', 'M/d/yyyy', null, 120),
      this.utilityFunctionsService.createDecimalColumnDef('Last Voltage Reading (mV)', 'LastVoltageReading', null, 0, true, FieldDefinitionTypeEnum.SensorLastVoltageReading),
      this.utilityFunctionsService.createDateColumnDef('Last Voltage Reading Date', 'LastVoltageReadingDate', 'M/d/yyyy', null, 120, FieldDefinitionTypeEnum.SensorLastVoltageReadingDate),
      { 
        headerName: 'Sensor Type', field: 'SensorTypeName',
        filter: CustomDropdownFilterComponent,
        filterParams: {
          field: 'SensorTypeName'
        },
        headerComponent: FieldDefinitionGridHeaderComponent, headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.SensorType },
      },
      {
        headerName: "Always On/Off",
        headerComponent: FieldDefinitionGridHeaderComponent,
        headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.ContinuityMeterStatus, labelOverride: 'Always On/Off' },
        valueGetter: params => params.data.SensorTypeID == SensorTypeEnum.ContinuityMeter ?
          params.data.SnoozeStartDate ? 'Snoozed' : params.data.ContinuityMeterStatus?.ContinuityMeterStatusDisplayName : 'N/A',
        filter: CustomDropdownFilterComponent,
        filterValueGetter: function (params: any) {
          return params.data.SensorTypeID == SensorTypeEnum.ContinuityMeter ? 
            params.data.SnoozeStartDate ? 'Snoozed' : params.data.ContinuityMeterStatus?.ContinuityMeterStatusDisplayName : 'N/A'
        },
      },
      { headerName: 'Latitude', field: 'Latitude' },
      { headerName: 'Longitude', field: 'Longitude' }
    ];
  }
  
  public downloadCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.wellsGrid, 'wells.csv', null);
  }

  public onSelectionChanged(event: Event) {
    const selectedNode = this.wellsGrid.api.getSelectedNodes()[0];
    if (!selectedNode) {
      // event was fired automatically when we updated the grid after a map click
      return;
    }
    this.wellsGrid.api.forEachNode(node => {
      if (node.data.WellRegistrationID === selectedNode.data.WellRegistrationID) {
        node.setSelected(true);
      }
    })
    this.wellMap.selectWell(selectedNode.data.WellRegistrationID);
  }

  public onMapSelection(wellRegistrationID: string) {
    this.wellsGrid.api.deselectAll();

    this.wellsGrid.api.forEachNode(node => {
      if (node.data.WellRegistrationID === wellRegistrationID) {
        node.setSelected(true);
        this.wellsGrid.api.ensureIndexVisible(node.rowIndex, "top");
      }
    })
  }
}

