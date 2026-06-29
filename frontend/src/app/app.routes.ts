import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'tracks', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () =>
      import('./components/login/login.component').then((m) => m.LoginComponent),
  },
  {
    path: 'tracks',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/track-list/track-list.component').then((m) => m.TrackListComponent),
  },
  {
    path: 'tracks/new',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/track-create/track-create.component').then((m) => m.TrackCreateComponent),
  },
  {
    path: 'tracks/:id',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/track-detail/track-detail.component').then((m) => m.TrackDetailComponent),
  },
  {
    path: 'artists',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/artist-list/artist-list.component').then((m) => m.ArtistListComponent),
  },
  {
    path: 'artists/new',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/artist-create/artist-create.component').then((m) => m.ArtistCreateComponent),
  },
  { path: '**', redirectTo: 'tracks' },
];
