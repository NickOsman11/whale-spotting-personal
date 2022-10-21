const backendUrl = process.env["REACT_APP_BACKEND_DOMAIN"];

export interface ListResponse<T> {
  items: T[];
}

export interface ConservationStatus {
  id: number;
  code: string;
  description: string;
}

export interface Species {
  id: number;
  name: string;
  scientificName: string;
  description: string;
  photoUrl: string;
  conservationStatus: ConservationStatus;
}

export interface SightingWithLocations {
  id: number;
  seenBy: string;
  seenOn: string;
  species?: Species;
  imageUrl: string;
  description: string;
  whaleCount: number;
  confirmationStatus: string;
  location: Location;
  latitude: number;
  longitude: number;
}

interface Location {
  id: number;
  description: string;
  species: string;
}

export interface User {
  id: number;
  name: string;
  email: string;
  userName: string;
}

export const getAllSpecies = async (): Promise<Species[]> => {
  const response = await fetch(`${backendUrl}/whales`);
  const whaleListResponse: ListResponse<Species> = await response.json();
  return whaleListResponse.items;
};

export const getSightings = async (): Promise<SightingWithLocations[]> => {
  const response = await fetch(`${backendUrl}/sightings`);
  const sightingsListResponse: ListResponse<SightingWithLocations> =
    await response.json();
  return sightingsListResponse.items;
};

export const getSightingsBySpeciesId = async (
  speciesId: string
): Promise<SightingWithLocations[]> => {
  const response = await fetch(`${backendUrl}/sightings/species/${speciesId}`);
  const sightingsListResponse: ListResponse<SightingWithLocations> =
    await response.json();
  return sightingsListResponse.items;
};

export const checkLogInDetails = async (
  username: string,
  password: string
): Promise<boolean> => {
  const response = await fetch(`${backendUrl}/login`, {
    headers: {
      Authorization: `Basic ${btoa(`${username}:${password}`)}`,
    },
  });
  return response.ok;
};
