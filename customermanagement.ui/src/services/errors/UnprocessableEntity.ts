import ApiError from './ApiError';

export default class UnprocessableEntity extends ApiError {
  constructor(message?: string) {
    const calculatedMessage =
      message === undefined ? 'Unable to complete the request' : message;
    super('Unprocessable Entity', calculatedMessage, new Error().stack);

  }
}