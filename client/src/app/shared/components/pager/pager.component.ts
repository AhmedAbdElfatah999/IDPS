import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {
  @Input() totalCount: number;
  @Input() pageSize: number;
  @Output() pageChaned = new EventEmitter<number>();

  constructor() { }

  ngOnInit(): void {
  }
  // tslint:disable-next-line: typedef
  onPagerChanged(event: any){
    this.pageChaned.emit(event.page);
  }

}
