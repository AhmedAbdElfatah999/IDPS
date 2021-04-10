import { Component, OnInit } from '@angular/core';
import {PaginationModule} from 'ngx-bootstrap/pagination';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'client';
  ngOnInit(): void{}
}
