export interface Artist {
  id: number;
  name: string;
  email: string;
  country: string;
}

export interface CreateArtistRequest {
  name: string;
  email: string;
  country: string;
}
