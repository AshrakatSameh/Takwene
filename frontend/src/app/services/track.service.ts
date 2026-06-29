import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  CreateTrackRequest,
  DistributeRequest,
  TrackDetail,
  TrackListItem,
  TrackStatus,
  UpdateTrackStatusRequest,
} from '../models/track.models';

@Injectable({ providedIn: 'root' })
export class TrackService {
  private readonly baseUrl = `${environment.apiUrl}/tracks`;

  constructor(private http: HttpClient) {}

  getTracks(status?: TrackStatus): Observable<TrackListItem[]> {
    let params = new HttpParams();
    if (status) {
      params = params.set('status', status);
    }
    return this.http.get<TrackListItem[]>(this.baseUrl, { params });
  }

  getTrack(id: number): Observable<TrackDetail> {
    return this.http.get<TrackDetail>(`${this.baseUrl}/${id}`);
  }

  createTrack(request: CreateTrackRequest): Observable<TrackDetail> {
    return this.http.post<TrackDetail>(this.baseUrl, request);
  }

  distribute(id: number, request: DistributeRequest): Observable<TrackDetail> {
    return this.http.post<TrackDetail>(`${this.baseUrl}/${id}/distribute`, request);
  }

  updateStatus(id: number, request: UpdateTrackStatusRequest): Observable<TrackDetail> {
    return this.http.patch<TrackDetail>(`${this.baseUrl}/${id}/status`, request);
  }
}
