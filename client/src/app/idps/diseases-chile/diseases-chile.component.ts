import { Component, Input, OnInit } from '@angular/core';
import { IDiseases } from 'src/app/shared/models/diseases';

@Component({
  selector: 'app-diseases-chile',
  templateUrl: './diseases-chile.component.html',
  styleUrls: ['./diseases-chile.component.scss']
})
export class DiseasesChileComponent implements OnInit {

  @Input() disease: IDiseases;

  constructor() { }

  ngOnInit(): void {
  }

}
