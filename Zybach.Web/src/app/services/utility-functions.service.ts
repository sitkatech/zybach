import { DatePipe, DecimalPipe } from '@angular/common';
import { Injectable } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { CsvExportParams } from 'ag-grid-community';
import { FieldDefinitionGridHeaderComponent } from '../shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { FieldDefinitionTypeEnum } from '../shared/generated/enum/field-definition-type-enum';

@Injectable({
  providedIn: 'root'
})
export class UtilityFunctionsService {

  constructor(
    private datePipe: DatePipe,
    private decimalPipe: DecimalPipe
  ) { }

  private decimalValueGetter(params: any, fieldName, allowNullData?: boolean): number {
    const fieldNames = fieldName.split('.');
    if (fieldNames.length == 1) {
      return params.data[fieldName] ?? (allowNullData ? null : 0);
    }

    // checks that each part of a nested field is not null
    var fieldValue = params.data;
    fieldNames.forEach(x => {
      fieldValue = fieldValue[x];
      if (!fieldValue) {
        fieldValue = allowNullData ? null : 0;
        return;
      }
    });

    return fieldValue;
  }

  public createDecimalColumnDef(headerName: string, fieldName: string, width?: number, decimalPlacesToDisplay?: number, allowNullData?: boolean, fieldDefinitionTypeID?: number) {
    const _decimalPipe = this.decimalPipe;
    const decimalFormatString = decimalPlacesToDisplay != null ? 
      '1.' + decimalPlacesToDisplay + '-' + decimalPlacesToDisplay : '1.2-2';
  
    var decimalColDef: ColDef = {
      headerName: headerName, filter: 'agNumberColumnFilter', cellStyle: { textAlign: 'right' }, sortable: true, resizable: true,
      valueGetter: params => this.decimalValueGetter(params, fieldName, allowNullData),
      valueFormatter: params => params.value ? _decimalPipe.transform(params.value, decimalFormatString) : '-',
    }
    if (width) {
      decimalColDef.width = width
    }
    if (fieldDefinitionTypeID) {
      decimalColDef.headerComponentFramework = FieldDefinitionGridHeaderComponent;
      decimalColDef.headerComponentParams = { fieldDefinitionTypeID: fieldDefinitionTypeID }
    }
  
    return decimalColDef;
  }

  private dateFilterComparator(filterLocalDateAtMidnight, cellValue) {
    const filterDate = Date.parse(filterLocalDateAtMidnight);
    const cellDate = Date.parse(cellValue);

    if (cellDate == filterDate) {
      return 0;
    }
    return (cellDate < filterDate) ? -1 : 1;
  }

  private dateSortComparator (id1: any, id2: any) {
    const date1 = id1 ? Date.parse(id1) : Date.parse("1/1/1900");
    const date2 = id2 ? Date.parse(id2) : Date.parse("1/1/1900");
    if (date1 < date2) {
      return -1;
    }
    return (date1 > date2)  ?  1 : 0;
  }

  public createDateColumnDef(headerName: string, fieldName: string, dateFormat: string, dateTimezone?: string, width?: number, fieldDefinitionTypeID?: number): ColDef {
    const _datePipe = this.datePipe;
    var dateColDef: ColDef = {
      headerName: headerName, valueGetter: function (params: any) {
        return _datePipe.transform(params.data[fieldName], dateFormat, dateTimezone);
      },
      comparator: this.dateSortComparator,
      filter: 'agDateColumnFilter',
      filterParams: {
        filterOptions: ['inRange'],
        comparator: this.dateFilterComparator
      }, 
      width: 110,
      resizable: true,
      sortable: true
    };
    if (width) {
      dateColDef.width = width;
    }
    if (fieldDefinitionTypeID) {
      dateColDef.headerComponentFramework = FieldDefinitionGridHeaderComponent;
      dateColDef.headerComponentParams = { fieldDefinitionTypeID: fieldDefinitionTypeID }
    }

    return dateColDef;
  }

  public exportGridToCsv(grid: AgGridAngular, fileName: string, columnKeys: Array<string>) {
    var params =
      {
        skipHeader: false,
        columnGroups: false,
        skipFooters: true,
        skipGroups: true,
        skipPinnedTop: true,
        skipPinnedBottom: false,
        allColumns: true,
        onlySelected: false,
        suppressQuotes: false,
        fileName: fileName,
        processCellCallback: function (p) {
          if (p.column.getColDef().cellRendererFramework) {
            if (p.value.DownloadDisplay) {
              return p.value.DownloadDisplay;
            } else {
              return p.value.LinkDisplay;
            }
          }
          else {
            return p.value;
          }
        }
      } as CsvExportParams
    if (columnKeys) {
      params.columnKeys = columnKeys;
    }
    grid.api.exportDataAsCsv(params);
  }

  public linkRendererComparator(id1: any, id2: any) {
    if (id1.LinkDisplay == id2.LinkDisplay) {
      return 0;
    }
    return id1.LinkDisplay > id2.LinkDisplay ? 1 : -1;
  }

  public multiLinkRendererComparator(id1: any, id2: any) {
    const value1 = id1.links.map(x => x.LinkDisplay).join(',');
    const value2 = id2.links.map(x => x.LinkDisplay).join(',');

    if (value1 == value2) {
      return 0;
    }
    return value1 > value2 ? 1 : -1;
  }
}
