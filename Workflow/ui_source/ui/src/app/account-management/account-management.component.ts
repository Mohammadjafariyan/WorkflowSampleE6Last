import {Component, OnInit} from '@angular/core';
import {FormGroup} from "@angular/forms";
import {ResultType, MyAccount,  VacationType} from "../service/models";
import {FormlyFieldConfig} from "@ngx-formly/core";
import {AccountService} from "../service/generic-service.service";

@Component({
  selector: 'app-account-management',
  templateUrl: './account-management.component.html',
  styleUrls: ['./account-management.component.css']
})
export class AccountManagementComponent implements OnInit {
  form = new FormGroup({});
  records: MyAccount[] = [];

  model = new MyAccount();
  fields: FormlyFieldConfig[];

  submit(model: MyAccount) {
    console.log(model);


    this.accountService.save(model).toPromise().then(res => {
      if (res.Type == ResultType.Success) {
        this.success(res.Message);
        this.ngOnInit();
      } else if (res.Type == ResultType.Fail) {
        this.fail(res.Message);
      }
    })

  }

  constructor(private accountService: AccountService) {
  }

  ngOnInit() {
    this.accountService.getAll().toPromise().then(this.parse)
      .then(res => {
        this.records = res;

        let options=[];
        for (let i = 0; i < this.records.length; i++) {
          options.push({label: this.records[i].Username, value: this.records[i].Id});
        }

        this.fields = [{
          key: 'Username',
          type: 'input',
          templateOptions: {
            label: 'نام کاربری',
            placeholder: 'مثال: amin6',
            required: true,
          }
        }, {
          key: 'ParentId',
          type: 'select',
          templateOptions: {
            label: 'نام کاربری بالارده',
            placeholder: 'انتخاب نمایید',
            required: true,
            options:options
          }
        }];
      })
  }

  private parse(res): MyAccount[] {
    if (res.Type == ResultType.Success) {
      return res.Grid;
    } else {
      alert(res.Message);
      return [];
    }
  }

  private success(Message) {
    alert('با موفقیت انجام شد');
  }

  private fail(Message) {
    alert(Message + 'خطا در ثبت :');
  }

  delete(m: MyAccount) {
    this.accountService.delete(m.Id).toPromise().then(res => {
      if (res.Type == ResultType.Success) {
        this.success(res.Message);
        this.ngOnInit();
      } else if (res.Type == ResultType.Fail) {
        this.fail(res.Message);
      }
    })


  }

}
