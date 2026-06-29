import { HttpErrorResponse } from '@angular/common/http';

// Flattens an RFC 7807 ProblemDetails / ValidationProblemDetails response
// into a list of human-readable messages for display.
export function extractApiErrors(err: HttpErrorResponse): string[] {
  const body = err?.error;
  if (err?.status === 401 || err?.status === 403) {
    return ['You need to log in to perform this action.'];
  }
  if (body?.errors && typeof body.errors === 'object') {
    return Object.values(body.errors).flat() as string[];
  }
  if (body?.detail) return [body.detail];
  if (body?.title) return [body.title];
  if (err?.status === 0) return ['Cannot reach the API. Is it running?'];
  return ['Something went wrong. Please try again.'];
}
