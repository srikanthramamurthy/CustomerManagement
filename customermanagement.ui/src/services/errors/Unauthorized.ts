import ApiError from './ApiError';

export default class Unauthorized extends ApiError {
  constructor(message?: string) {
    const calculatedMessage =
      message === undefined ? 'Session time out' : message;
    super('Unauthorized', calculatedMessage, new Error().stack);
  }
}
