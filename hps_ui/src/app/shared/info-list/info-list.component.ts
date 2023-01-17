import { Component, OnInit } from '@angular/core';
import { ServiceService } from 'src/app/Services/service.service';

@Component({
  selector: 'app-info-list',
  templateUrl: './info-list.component.html',
  styleUrls: ['./info-list.component.scss']
})
export class InfoListComponent implements OnInit {

  infoList: any;

  constructor(private action: ServiceService) { }

  ngOnInit(): void {
  }

  getAll(){
    this.action.getAllInfo().subscribe(res => {
      console.log(res);
      this.infoList = res;
    })
  }

}
