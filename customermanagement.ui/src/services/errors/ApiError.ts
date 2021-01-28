export default class ApiError {
  public name: string;
  public message: string;
  public stack: string | undefined;

  constructor(name: string, message: string, stack: string | undefined) {
    this.name = name;
    this.message = message;
    this.stack = stack;
  }
}
