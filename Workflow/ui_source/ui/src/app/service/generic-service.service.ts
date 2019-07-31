import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {SingleTon} from "./SingleTon";
import {Observable} from "rxjs";
import {InboxTaskViewModel, IWorkflowModel, MyAccount, Result, Vacation, VoidResult} from "./models";

@Injectable({
  providedIn: 'root'
})
export class GenericService<M extends IWorkflowModel> {
  controllerName;

  constructor(private http: HttpClient, private singleTon: SingleTon) {
  }

  getAll(): Observable<Result<M>> {
    return this.http.get<Result<M>>(`${this.singleTon.baseUrl}/${this.controllerName}/getAll`);
  }

  getById(id): Observable<Result<M>> {
    return this.http.get<Result<M>>(`${this.singleTon.baseUrl}/${this.controllerName}/getAll?id=${id}`);
  }

  delete(id): Observable<VoidResult> {
    return this.http.post<VoidResult>(`${this.singleTon.baseUrl}/${this.controllerName}/delete/${id}`, {id: id});
  }

  save(model: M): Observable<VoidResult> {
    return this.http.post<VoidResult>
    (`${this.singleTon.baseUrl}/${this.controllerName}/save`, model);
  }

  saveAndSendToWorkflow(model: M): Observable<VoidResult> {
    return this.http.post<VoidResult>
    (`${this.singleTon.baseUrl}/${this.controllerName}/SaveAndSendToWorkflow`, model);
  }


  getInboxTasks(isClaimed): Observable<Result<InboxTaskViewModel<M>>> {
    return this.http.get<Result<InboxTaskViewModel<M>>>
    (`${this.singleTon.baseUrl}/${this.controllerName}/GetInboxTasks?isClaimed=${isClaimed}`);
  }


  claim(taskId) {
    return this.http.post<Result<InboxTaskViewModel<M>>>
    (`${this.singleTon.baseUrl}/${this.controllerName}/claim?taskId=${taskId}`,{taskId:taskId});
  }

  unClaim(taskId) {
    return this.http.post<Result<InboxTaskViewModel<M>>>
    (`${this.singleTon.baseUrl}/${this.controllerName}/unClaim?taskId=${taskId}`,{taskId:taskId});
  }

  continue(taskId, param2) {
    param2={name:'hi',value:'32'};
    return this.http.post<Result<InboxTaskViewModel<M>>>
    (`${this.singleTon.baseUrl}/${this.controllerName}/Continue?taskId=${taskId}`,{vars:param2});
  }

  getDiagram(processInstanceId):Observable<Result<any>> {
    const param:any={responseType:'blob'};
    return this.http.post<Result<any>>
    (`${this.singleTon.baseUrl}/${this.controllerName}/getDiagram?processInstanceId=${processInstanceId}` , {processInstanceId:processInstanceId});
  }

}


@Injectable({
  providedIn: 'root'
})
export class VacationService extends GenericService<Vacation> {
  controllerName = 'vacation';



}

@Injectable({
  providedIn: 'root'
})
export class AccountService extends GenericService<MyAccount> {
  controllerName = 'Account';
}


