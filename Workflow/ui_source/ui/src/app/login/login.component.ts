import {Component, OnInit} from '@angular/core';
import {FormGroup} from "@angular/forms";
import {ResultType, Vacation, VacationType, VoidResult} from "../service/models";
import {FormlyFieldConfig} from "@ngx-formly/core";
import {HttpClient} from "@angular/common/http";
import {SingleTon} from "../service/SingleTon";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form = new FormGroup({});
  records: Vacation[] = [];
  type = VacationType;

  model = new Vacation();
  fields: FormlyFieldConfig[] = [{
    key: 'username',
    type: 'input',
    templateOptions: {
      label: 'نام کاربری جهت ورود به سیستم :',
      placeholder: 'مثال: amin6',
      required: true,
    }
  }];


  constructor(private http: HttpClient,
              public singleTon: SingleTon) {
  }

  ngOnInit() {
  }


  submit(model: Vacation) {
    console.log(model);


    this.http.get<VoidResult>(`${this.singleTon.baseUrl}/account/login?username=${this.form.value.username}`).toPromise().then(res => {
      if (res.Type == ResultType.Success) {
        this.singleTon.loggedInUsername = this.form.value.username;
      } else {
        alert(res.Message);
      }
    })

  }

}
