import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { LucideAngularModule, ArrowLeft, Disc3 } from 'lucide-angular';
import { TrackService } from '../../services/track.service';
import { ArtistService } from '../../services/artist.service';
import { Artist } from '../../models/artist.models';
import { extractApiErrors } from '../../utils/api-error';

@Component({
  selector: 'app-track-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LucideAngularModule],
  templateUrl: './track-create.component.html',
})
export class TrackCreateComponent implements OnInit {
  readonly ArrowLeft = ArrowLeft;
  readonly Disc3 = Disc3;

  private fb = inject(FormBuilder);
  private trackService = inject(TrackService);
  private artistService = inject(ArtistService);
  private router = inject(Router);

  artists: Artist[] = [];
  submitting = false;
  errors: string[] = [];

  form = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(300)]],
    artistId: [null as number | null, [Validators.required]],
    isrc: ['', [Validators.required, Validators.pattern(/^[A-Za-z0-9]{12}$/)]],
    releaseDate: ['', [Validators.required]],
    genre: ['', [Validators.required, Validators.maxLength(100)]],
  });

  ngOnInit(): void {
    this.artistService.getArtists().subscribe({
      next: (data) => (this.artists = data),
      error: (err) => (this.errors = extractApiErrors(err)),
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.submitting = true;
    this.errors = [];
    this.trackService.createTrack(this.form.value as any).subscribe({
      next: (track) => this.router.navigate(['/tracks', track.id]),
      error: (err) => {
        this.errors = extractApiErrors(err);
        this.submitting = false;
      },
    });
  }
}
