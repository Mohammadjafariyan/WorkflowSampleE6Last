import { Component, OnInit } from '@angular/core';
import {
  BpmnModelViewModel,
  FormProperty,
  ProcessDefinition,
  ProcessInstanceStatus,
  ResultType
} from "../service/models";
import {DataHolderService, ProcessDefinitionService, WorkflowService} from "../service/generic-service.service";
import {Router} from "@angular/router";
import {SingleTon} from "../service/SingleTon";

@Component({
  selector: 'app-receivers',
  templateUrl: './receivers.component.html',
  styleUrls: ['./receivers.component.css']
})
export class ReceiversComponent implements OnInit {


  records: FormProperty[] = [];
  type = ProcessInstanceStatus;
  imgUrl;

  private imageToShow: string | ArrayBuffer;


  constructor(private processDefinitionService: ProcessDefinitionService,
              private workflowService: WorkflowService,
              private dataHolderService: DataHolderService,
              private router:Router,
              private singleTon:SingleTon) {
  }

  ngOnInit() {

    const id= this.dataHolderService.processDefinition.id;
    const key= this.dataHolderService.processDefinition.key;


    this.imgUrl=`http://localhost:8090/getPhoto?key=${key}`;

    // کارتابل عمومی
    this.workflowService.GetModel(id).toPromise()
      .then(res => {
        if (res.Type == ResultType.Success) {
          this.records = res.Grid;
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
