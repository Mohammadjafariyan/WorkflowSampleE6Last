import {Component} from '@angular/core';
import {SingleTon} from "./service/SingleTon";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [SingleTon]
})
export class AppComponent {
  title = 'ui';


  constructor(private SingleTon: SingleTon) {

  }
}
