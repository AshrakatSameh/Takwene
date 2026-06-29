import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { LucideAngularModule, ArrowLeft, Disc3, Radio, Send, RefreshCw } from 'lucide-angular';
import { TrackService } from '../../services/track.service';
import { DspService } from '../../services/dsp.service';
import { AuthService } from '../../services/auth.service';
import { TrackDetail, TrackStatus } from '../../models/track.models';
import { Dsp } from '../../models/dsp.models';
import { extractApiErrors } from '../../utils/api-error';

@Component({
  selector: 'app-track-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, LucideAngularModule],
  templateUrl: './track-detail.component.html',
})
export class TrackDetailComponent implements OnInit {
  readonly ArrowLeft = ArrowLeft;
  readonly Disc3 = Disc3;
  readonly Radio = Radio;
  readonly Send = Send;
  readonly RefreshCw = RefreshCw;

  private route = inject(ActivatedRoute);
  private trackService = inject(TrackService);
  private dspService = inject(DspService);
  private auth = inject(AuthService);

  readonly isAuthenticated = this.auth.isAuthenticated;
  readonly statuses: TrackStatus[] = ['Draft', 'Submitted', 'Distributed'];

  track?: TrackDetail;
  dsps: Dsp[] = [];
  loading = false;
  error = '';

  selectedDspIds = new Set<number>();
  statusValue: TrackStatus = 'Draft';
  working = false;
  actionErrors: string[] = [];
  actionMessage = '';

  private id!: number;

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadTrack();
    if (this.isAuthenticated()) {
      this.dspService.getDsps().subscribe({ next: (d) => (this.dsps = d), error: () => {} });
    }
  }

  loadTrack(): void {
    this.loading = true;
    this.trackService.getTrack(this.id).subscribe({
      next: (data) => {
        this.track = data;
        this.statusValue = data.status;
        this.loading = false;
      },
      error: (err) => {
        this.error = err.status === 404 ? 'Track not found.' : 'Could not load this track.';
        this.loading = false;
      },
    });
  }

  toggleDsp(id: number): void {
    if (this.selectedDspIds.has(id)) this.selectedDspIds.delete(id);
    else this.selectedDspIds.add(id);
  }

  distribute(): void {
    if (this.selectedDspIds.size === 0) return;
    this.working = true;
    this.actionErrors = [];
    this.actionMessage = '';
    this.trackService.distribute(this.id, { dspIds: [...this.selectedDspIds] }).subscribe({
      next: (data) => {
        this.track = data;
        this.statusValue = data.status;
        this.selectedDspIds.clear();
        this.actionMessage = 'Track submitted to the selected DSP(s).';
        this.working = false;
      },
      error: (err) => {
        this.actionErrors = extractApiErrors(err);
        this.working = false;
      },
    });
  }

  updateStatus(): void {
    this.working = true;
    this.actionErrors = [];
    this.actionMessage = '';
    this.trackService.updateStatus(this.id, { status: this.statusValue }).subscribe({
      next: (data) => {
        this.track = data;
        this.actionMessage = 'Status updated.';
        this.working = false;
      },
      error: (err) => {
        this.actionErrors = extractApiErrors(err);
        this.working = false;
      },
    });
  }

  trackStatusClass(status: string): string {
    switch (status) {
      case 'Distributed':
        return 'bg-green-50 text-green-700 border-green-200';
      case 'Submitted':
        return 'bg-amber-50 text-amber-700 border-amber-200';
      default:
        return 'bg-gray-100 text-gray-600 border-gray-200';
    }
  }

  distStatusClass(status: string): string {
    switch (status) {
      case 'Live':
        return 'bg-green-50 text-green-700 border-green-200';
      case 'Pending':
        return 'bg-amber-50 text-amber-700 border-amber-200';
      default:
        return 'bg-red-50 text-red-700 border-red-200';
    }
  }
}
