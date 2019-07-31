import {Component, OnInit} from '@angular/core';
import {FormGroup} from "@angular/forms";
import {FormlyFieldConfig} from "@ngx-formly/core";
import {ResultType, Vacation, VacationType} from "../service/models";
import {VacationService} from "../service/generic-service.service";

@Component({
  selector: 'app-vacation-request',
  templateUrl: './vacation-request.component.html',
  styleUrls: ['./vacation-request.component.css']
})
export class VacationRequestComponent implements OnInit {
  form = new FormGroup({});
  records: Vacation[] = [];
  type = VacationType;

  model = new Vacation();
  fields: FormlyFieldConfig[] = [{
    key: 'title',
    type: 'input',
    templateOptions: {
      label: 'عنوان مرخصی را اینجا وارد نمایید',
      placeholder: 'مثال: مرخصی جهت سفر به مشهد',
      required: true,
    }
  }, {
    key: 'type',
    type: 'select',
    templateOptions: {
      label: 'نوع را اینجا وارد نمایید',
      required: true,
      options: [
        {label: 'استحقاقی', value: VacationType.استحقاقی},
        {label: 'استعلاجی', value: VacationType.استعلاجی},
        {label: 'اشعه', value: VacationType.اشعه},
      ],
    }
  }];

  submit(model: Vacation) {
    console.log(model);


    this.vacationService.save(model).toPromise().then(res => {
      if (res.Type == ResultType.Success) {
        this.success(res.Message);
        this.ngOnInit();
      } else if (res.Type == ResultType.Fail) {
        this.fail(res.Message);
      }
    })

  }

  constructor(private vacationService: VacationService) {
  }

  ngOnInit() {
    this.vacationService.getAll().toPromise().then(this.parse)
      .then(res => {
        this.records = res;
      })
  }

  private parse(res): Vacation[] {
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

  delete(m: Vacation) {
    this.vacationService.delete(m.Id).toPromise().then(res => {
      if (res.Type == ResultType.Success) {
        this.success(res.Message);
        this.ngOnInit();
      } else if (res.Type == ResultType.Fail) {
        this.fail(res.Message);
      }
    })


  }

  sendtoWorkflow(m: Vacation) {

  }

  SaveAndSendToWorkflow() {
   const m= this.form.value;

    this.vacationService.saveAndSendToWorkflow(m).toPromise().then(res => {
      if (res.Type == ResultType.Success) {
        this.success(res.Message);
        this.ngOnInit();
      } else if (res.Type == ResultType.Fail) {
        this.fail(res.Message);
      }
    })
  }
}
