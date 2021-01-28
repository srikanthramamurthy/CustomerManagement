import ApiError from './ApiError';

export default class ForbiddenError extends ApiError {
  constructor(message?: string) {
    const calculatedMessage =
      message === undefined ? 'Forbidden access to resource' : message;
    super('Forbidden Error', calculatedMessage, new Error().stack);
  }
}
