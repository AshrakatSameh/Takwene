import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { LucideAngularModule, Users, Plus, Mail, MapPin } from 'lucide-angular';
import { ArtistService } from '../../services/artist.service';
import { Artist } from '../../models/artist.models';

@Component({
  selector: 'app-artist-list',
  standalone: true,
  imports: [CommonModule, RouterLink, LucideAngularModule],
  templateUrl: './artist-list.component.html',
})
export class ArtistListComponent implements OnInit {
  readonly Users = Users;
  readonly Plus = Plus;
  readonly Mail = Mail;
  readonly MapPin = MapPin;

  private artistService = inject(ArtistService);

  artists: Artist[] = [];
  loading = false;
  error = '';

  ngOnInit(): void {
    this.loading = true;
    this.artistService.getArtists().subscribe({
      next: (data) => {
        this.artists = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Could not load artists. Make sure the API is running.';
        this.loading = false;
      },
    });
  }
}
