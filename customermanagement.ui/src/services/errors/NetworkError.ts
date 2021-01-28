import ApiError from './ApiError';

export default class NetworkError extends ApiError {
  constructor(message?: string) {
    const calculatedMessage =
      message === undefined ? 'Unable to connect to server' : message;
    super('Network Error', calculatedMessage, new Error().stack);
  }
}
