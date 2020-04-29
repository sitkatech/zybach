import { Component, OnInit } from '@angular/core';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum'

@Component({
  selector: 'drooltool-take-action',
  templateUrl: './take-action.component.html',
  styleUrls: ['./take-action.component.scss']
})
export class TakeActionComponent implements OnInit {
  public richTextTypeID : number = CustomRichTextType.TakeAction;
  constructor() { }

  ngOnInit() {
  }

}
