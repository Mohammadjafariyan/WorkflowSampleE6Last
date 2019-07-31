import {Injectable} from "@angular/core";


@Injectable({
  providedIn: 'root'
})
export class SingleTon {
  baseUrl: string="http://localhost:5000/api";
  loggedInUsername: any;

}
