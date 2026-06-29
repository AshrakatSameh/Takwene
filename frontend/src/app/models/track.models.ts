export type TrackStatus = 'Draft' | 'Submitted' | 'Distributed';
export type DistributionStatus = 'Pending' | 'Live' | 'Rejected';

export interface TrackListItem {
  id: number;
  title: string;
  artistName: string;
  genre: string;
  status: TrackStatus;
}

export interface Distribution {
  dspId: number;
  dspName: string;
  status: DistributionStatus;
  submittedAt: string;
}

export interface TrackDetail {
  id: number;
  title: string;
  artistId: number;
  artistName: string;
  isrc: string;
  releaseDate: string;
  genre: string;
  status: TrackStatus;
  distributions: Distribution[];
}

export interface CreateTrackRequest {
  title: string;
  artistId: number;
  isrc: string;
  releaseDate: string;
  genre: string;
}

export interface DistributeRequest {
  dspIds: number[];
}

export interface UpdateTrackStatusRequest {
  status: TrackStatus;
}
