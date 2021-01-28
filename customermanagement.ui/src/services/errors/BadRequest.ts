import ApiError from './ApiError';

export default class BadRequest extends ApiError {
  constructor(message?: string) {
    const calculatedMessage =
      message === undefined ? 'Unable to complete the request' : message;
    super('Bad Request', calculatedMessage, new Error().stack);
  }
}
