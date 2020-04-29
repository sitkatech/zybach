import { Component, OnInit } from '@angular/core';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum'

@Component({
  selector: 'drooltool-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss']
})
export class AboutComponent implements OnInit {
  public richTextTypeID : number = CustomRichTextType.About;
  constructor() { }

  ngOnInit() {
  }

}
