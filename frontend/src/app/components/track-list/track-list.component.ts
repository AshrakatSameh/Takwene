import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { LucideAngularModule, Disc3, ChevronRight, Music2, Plus } from 'lucide-angular';
import { TrackService } from '../../services/track.service';
import { TrackListItem, TrackStatus } from '../../models/track.models';

type Filter = TrackStatus | 'All';

@Component({
  selector: 'app-track-list',
  standalone: true,
  imports: [CommonModule, RouterLink, LucideAngularModule],
  templateUrl: './track-list.component.html',
})
export class TrackListComponent implements OnInit {
  readonly Disc3 = Disc3;
  readonly ChevronRight = ChevronRight;
  readonly Music2 = Music2;
  readonly Plus = Plus;

  tracks: TrackListItem[] = [];
  loading = false;
  error = '';

  activeFilter: Filter = 'All';
  readonly filters: Filter[] = ['All', 'Draft', 'Submitted', 'Distributed'];

  constructor(private trackService: TrackService) {}

  ngOnInit(): void {
    this.load();
  }

  setFilter(f: Filter): void {
    if (this.activeFilter === f) return;
    this.activeFilter = f;
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = '';
    const status = this.activeFilter === 'All' ? undefined : this.activeFilter;
    this.trackService.getTracks(status).subscribe({
      next: (data) => {
        this.tracks = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Could not load tracks. Make sure the API is running at the configured URL.';
        this.loading = false;
      },
    });
  }

  statusClass(status: string): string {
    switch (status) {
      case 'Distributed':
        return 'bg-green-50 text-green-700 border-green-200';
      case 'Submitted':
        return 'bg-amber-50 text-amber-700 border-amber-200';
      default:
        return 'bg-gray-100 text-gray-600 border-gray-200'; // Draft
    }
  }
}
