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


export interface BpmnModelViewModel extends  IWorkflowModel{
  Id;
  Name;
  Username;
  Post;
}


export interface FormProperty extends  IWorkflowModel
{
  Post;
  Username;
  id;
  xmlRowNumber;
  xmlColumnNumber;
  extensionElements;
  attributes;
  name;
  expression;
  variable;
  type:TypeEnum;
  defaultExpression;
  datePattern;
  readable;
  writeable;
  required;
  formValues;
}

export enum TypeEnum { Long, String }


export interface ProcessDefinitionGrid extends  IWorkflowModel{
  data:  ProcessDefinition[];
  total: number;
  start: number;
  sort:  string;
  order: string;
  size:  number;
}

export interface ProcessDefinition {
  id:                       string;
  url:                      string;
  version:                  number;
  key:                      string;
  category:                 string;
  suspended:                boolean;
  name:                     string;
  description:              string;
  deploymentID:             string;
  deploymentURL:            string;
  graphicalNotationDefined: boolean;
  resource:                 string;
  diagramResource:          string;
  startFormDefined:         boolean;
}

export class Notification implements IWorkflowModel
{
  Id;
  Subject;
  ReceiverUsername;
  Title;
  Description;
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
