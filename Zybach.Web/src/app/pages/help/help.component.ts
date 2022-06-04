import { Component, OnInit } from '@angular/core';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';

@Component({
  selector: 'zybach-help',
  templateUrl: './help.component.html',
  styleUrls: ['./help.component.scss']
})
export class HelpComponent implements OnInit {

  public richTextTypeID : number = CustomRichTextTypeEnum.Help;

  constructor() { }

  ngOnInit() {
  }

}
