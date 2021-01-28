import ApiError from './ApiError';

export default class NotFoundError extends ApiError {
  constructor(message?: string) {
    const calculatedMessage =
      message === undefined ? 'Unable to find resource' : message;
    super('Not Found Error', calculatedMessage, new Error().stack);
  }
}
