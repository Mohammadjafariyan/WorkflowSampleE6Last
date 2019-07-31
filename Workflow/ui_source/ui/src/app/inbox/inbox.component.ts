import { Component, OnInit } from '@angular/core';
import {FormGroup} from "@angular/forms";
import {InboxTaskViewModel, ProcessInstanceStatus, ResultType, Vacation, VacationType} from "../service/models";
import {FormlyFieldConfig} from "@ngx-formly/core";
import {VacationService} from "../service/generic-service.service";

@Component({
  selector: 'app-inbox',
  templateUrl: './inbox.component.html',
  styleUrls: ['./inbox.component.css']
})
export class InboxComponent implements OnInit {
  form = new FormGroup({});
  records: InboxTaskViewModel<Vacation>[] = [];
  type = ProcessInstanceStatus;

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
  generalRecords: InboxTaskViewModel<Vacation>[] = [];
  private imageToShow: string | ArrayBuffer;

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

    // کارتابل عمومی
    this.vacationService.getInboxTasks(false).toPromise()
      .then(res => {
        if (res.Type == ResultType.Success) {
          this.generalRecords = res.Grid;
        } else {
          this.records=[];
          alert(res.Message);
        }

      });


    this.vacationService.getInboxTasks(true).toPromise()
      .then(res => {
        if (res.Type == ResultType.Success) {
          this.records = res.Grid;
        } else {
          this.records=[];
          alert(res.Message);
        }

      });
  }



  private success(Message) {
    alert('با موفقیت انجام شد');
  }

  private fail(Message) {
    alert(Message + 'خطا در ثبت :');
  }


  claim(vm:InboxTaskViewModel<Vacation>) {
    this.vacationService.claim(vm.TaskId).toPromise()
      .then(res => {
        if (res.Type == ResultType.Success) {
          this.ngOnInit();
        } else {
          this.records=[];
          alert(res.Message);
        }

      });
  }

  unClaim(vm:InboxTaskViewModel<Vacation>) {
    this.vacationService.unClaim(vm.TaskId).toPromise()
      .then(res => {
        if (res.Type == ResultType.Success) {
          this.ngOnInit();
        } else {
          this.records=[];
          alert(res.Message);
        }

      });
  }

  continue(m: InboxTaskViewModel<Vacation>) {
    this.vacationService.continue(m.TaskId,null).toPromise()
      .then(res => {
        if (res.Type == ResultType.Success) {
          this.ngOnInit();
        } else {
          this.records=[];
          alert(res.Message);
        }

      });
  }

  showDiagram(m: InboxTaskViewModel<Vacation>) {
    this.vacationService.getDiagram(m.ProcessInstanceId).toPromise()
      .then(res => {
        if (res.Type == ResultType.Success) {
          const blob = new Blob([res.SingleRecord], {type: 'image/png'});

          this.createImageFromBlob(blob);
        } else {
          this.records=[];
          alert(res.Message);
        }

      });
  }


  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.imageToShow = reader.result;
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }
  }

}
