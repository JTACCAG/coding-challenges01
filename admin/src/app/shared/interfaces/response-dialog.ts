export class IResponseDialog<T = any> {
  isConfirmed!: boolean;
  isDenied!: boolean;
  isDismissed!: boolean;
  value?: T;
}
