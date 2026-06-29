import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Dsp } from '../models/dsp.models';

@Injectable({ providedIn: 'root' })
export class DspService {
  private readonly baseUrl = `${environment.apiUrl}/dsps`;

  constructor(private http: HttpClient) {}

  getDsps(): Observable<Dsp[]> {
    return this.http.get<Dsp[]>(this.baseUrl);
  }
}
