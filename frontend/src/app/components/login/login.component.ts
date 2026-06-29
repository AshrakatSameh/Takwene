import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LucideAngularModule, LogIn } from 'lucide-angular';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, LucideAngularModule],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  readonly LogIn = LogIn;

  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  submitting = false;
  error = '';

  form = this.fb.group({
    username: ['admin', Validators.required],
    password: ['Admin123!', Validators.required],
  });

  submit(): void {
    if (this.form.invalid) return;
    this.submitting = true;
    this.error = '';
    const { username, password } = this.form.value;
    this.auth.login(username!, password!).subscribe({
      next: () => {
        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') ?? '/tracks';
        this.router.navigateByUrl(returnUrl);
      },
      error: (err) => {
        this.error = err.status === 401 ? 'Invalid username or password.' : 'Login failed. Is the API running?';
        this.submitting = false;
      },
    });
  }
}
