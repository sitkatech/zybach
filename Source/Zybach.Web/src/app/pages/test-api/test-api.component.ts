import { Component, OnInit } from '@angular/core';
import { WellService } from 'src/app/services/well.service';

@Component({
  selector: 'zybach-test-api',
  templateUrl: './test-api.component.html',
  styleUrls: ['./test-api.component.scss']
})
export class TestAPIComponent implements OnInit {
  public results: string;
  constructor(private wellService: WellService) { }

  ngOnInit(): void {
    this.wellService.getWells().subscribe(x=>{
      this.results = JSON.stringify(x);
    }
    )
  }

}
