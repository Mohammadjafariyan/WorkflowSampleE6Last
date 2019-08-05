import {Component, OnInit} from '@angular/core';
import {FormGroup} from "@angular/forms";
import {
  InboxTaskViewModel, ProcessDefinition,
  ProcessDefinitionGrid,
  ProcessInstanceStatus,
  ResultType,
  Vacation,
  VacationType
} from "../service/models";
import {FormlyFieldConfig} from "@ngx-formly/core";
import {
  DataHolderService,
  ProcessDefinitionService,
  VacationService,
  WorkflowService
} from "../service/generic-service.service";
import {ActivatedRoute, Route, Router} from "@angular/router";

@Component({
  selector: 'app-process-definition-list',
  templateUrl: './process-definition-list.component.html',
  styleUrls: ['./process-definition-list.component.css']
})
export class ProcessDefinitionListComponent implements OnInit {

  records: ProcessDefinition[] = [];
  type = ProcessInstanceStatus;

  private imageToShow: string | ArrayBuffer;


  constructor(private processDefinitionService: ProcessDefinitionService,
              private workflowService: WorkflowService,
              private dataHolderService: DataHolderService,
              private router:Router) {
  }

  ngOnInit() {

    // کارتابل عمومی
    this.processDefinitionService.getAll().toPromise()
      .then(res => {
        if (res.Type == ResultType.Success) {
          this.records = res.SingleRecord.data;
        } else {
          this.records = [];
          alert(res.Message);
        }

      });


  }

  showDiagram(m: ProcessDefinition) {
    /*this.workflowService.GetPhoto(m.key).toPromise()
      .then(res => {
        if (res.Type == ResultType.Success) {
          const blob = new Blob([res.SingleRecord], {type: 'image/png'});

          this.createImageFromBlob(blob);
        } else {
          this.records=[];
          alert(res.Message);
        }

      });*/
  }


  getReceivers(m: ProcessDefinition) {
    this.dataHolderService.processDefinition = m;
    this.router.navigate(['/receivers']);
  }
}
