export class IResponseApi<T = object> {
  message!: string;
  error!: string;
  statusCode!: number;
  data!: T;
}
