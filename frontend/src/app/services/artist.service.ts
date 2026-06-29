import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Artist, CreateArtistRequest } from '../models/artist.models';

@Injectable({ providedIn: 'root' })
export class ArtistService {
  private readonly baseUrl = `${environment.apiUrl}/artists`;

  constructor(private http: HttpClient) {}

  getArtists(): Observable<Artist[]> {
    return this.http.get<Artist[]>(this.baseUrl);
  }

  createArtist(request: CreateArtistRequest): Observable<Artist> {
    return this.http.post<Artist>(this.baseUrl, request);
  }
}
