import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { LucideAngularModule, ArrowLeft, Users } from 'lucide-angular';
import { ArtistService } from '../../services/artist.service';
import { extractApiErrors } from '../../utils/api-error';

@Component({
  selector: 'app-artist-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LucideAngularModule],
  templateUrl: './artist-create.component.html',
})
export class ArtistCreateComponent {
  readonly ArrowLeft = ArrowLeft;
  readonly Users = Users;

  private fb = inject(FormBuilder);
  private artistService = inject(ArtistService);
  private router = inject(Router);

  submitting = false;
  errors: string[] = [];

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    email: ['', [Validators.required, Validators.email]],
    country: ['', [Validators.required, Validators.maxLength(100)]],
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.submitting = true;
    this.errors = [];
    this.artistService.createArtist(this.form.value as any).subscribe({
      next: () => this.router.navigate(['/artists']),
      error: (err) => {
        this.errors = extractApiErrors(err);
        this.submitting = false;
      },
    });
  }
}
