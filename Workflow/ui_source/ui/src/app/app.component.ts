import {Component} from '@angular/core';
import {SingleTon} from "./service/SingleTon";
import {DataHolderService} from "./service/generic-service.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [SingleTon,DataHolderService]
})
export class AppComponent {
  title = 'ui';


  constructor(private SingleTon: SingleTon,private DataHolderService:DataHolderService) {

  }
}
