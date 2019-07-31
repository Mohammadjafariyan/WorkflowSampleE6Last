export class VoidResult {
  Message;
  Type: ResultType;
}

export class Result<T> extends VoidResult {
  Grid: T[];
  SingleRecord: T;
}

export enum ResultType {
  Success,
  Fail
}


export class Vacation implements IWorkflowModel {
  Id;
  Title;
  Type: VacationType;
  Subject;
}

export interface IWorkflowModel {
  Id;
}


export enum VacationType {
  استحقاقی, استعلاجی, اشعه
}

export class MyAccount implements IWorkflowModel {
  Id;
  ParentId;
  Username;
  UpperUsername;
}

export enum ProcessInstanceStatus {
  Error,
  InProgress,
  Done
}

export class InboxTaskViewModel<T extends IWorkflowModel> {
  /// <summary>
  /// تایید کننده
  /// </summary>
  Assignee;

  ProcessInstanceId;
  TaskId;

  /// <summary>
  /// تایید کننده
  /// </summary>
  AssigneeTranslate;

  Subject;

  /// <summary>
  /// نام فرم
  /// </summary>
  FormName;

  RequestDate;

  Status: ProcessInstanceStatus;
  StatusMessage;
  Record: T;
}
