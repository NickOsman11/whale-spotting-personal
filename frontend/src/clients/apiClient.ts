const backendUrl = process.env.BACKEND_URL || "https://localhost:5001";

export interface ListResponse<T> {
  items: T[];
}

export interface SightingListResponse {
  sightings: ExternalSighting[];
}

export interface ExternalSighting {
  id: number;
  date: Date;
  location: ExternalLocation;
  species: ExternalSpecies[];
  photoUrl: string;
  email: string;
}

export interface ExternalLocation {
  id: number;
  latitude: number;
  longitude: number;
  name: string;
  description: string;
}

export interface ExternalSpecies {
  id: number;
  name: string;
  latinName: string;
  photoUrl: string;
  description: string;
  endangeredStatus: string;
}

export interface ConservationStatus {
  id: number;
  code: string;
  description: string;
}

export interface Location {
  id: number;
  description: string;
  species: Species[];
}

export interface Species {
  id: number;
  name: string;
  scientificName: string;
  description: string;
  photoUrl: string;
  conservationStatus: ConservationStatus;
  locations: Location[];
}

export interface Sighting {
  id: number;
  seenBy: string;
  seenOn: Date;
  species?: Species;
  imageUrl?: string;
  description?: string;
  whaleCount: number;
  location?: Location;
  latitude: number;
  longitude: number;
}

export type GenericSighting = Sighting | ExternalSighting;

export const isExternalSighting = (
  sighting: GenericSighting
): sighting is ExternalSighting => {
  return "date" in sighting;
};

export const getDate = (sighting: GenericSighting): Date => {
  return (sighting as Sighting).seenOn ?? (sighting as ExternalSighting).date;
};

export interface CreateSightingRequest {
  seenBy: string;
  seenOn: string;
  speciesId?: number;
  imageUrl: string;
  description: string;
  whaleCount: number;
  latitude: number;
  longitude: number;
}

export interface ConfirmOrRejectRequest {
  newConfirmationStatus: number;
}

export interface User {
  id: number;
  name: string;
  email: string;
  userName: string;
}

export interface CreateUserRequest {
  name: string;
  username: string;
  email: string;
  password: string;
}

export const getAllSpecies = async (): Promise<Species[]> => {
  const response = await fetch(`${backendUrl}/whales`);
  const whaleListResponse: ListResponse<Species> = await response.json();
  return whaleListResponse.items;
};

export const getAllLocations = async (): Promise<Location[]> => {
  const response = await fetch(`${backendUrl}/locations`);
  const locationListResponse: ListResponse<Location> = await response.json();
  return locationListResponse.items;
};

export const getAllPendingSightings = async (): Promise<Sighting[]> => {
  const response = await fetch(`${backendUrl}/sightings/pending`);
  const pendingSighting: ListResponse<Sighting> = await response.json();
  return pendingSighting.items;
};

export type ConfirmationStatus = "pending" | "approved" | "rejected";

export const confirmOrRejectSighting = async (
  sightingId: number,
  newStatus: ConfirmationStatus,
  username: string,
  password: string
): Promise<void> => {
  const statusMapping: { [key in ConfirmationStatus]: number } = {
    pending: 0,
    rejected: 1,
    approved: 2,
  };

  const confirmationCode: number = statusMapping[newStatus];

  const response = await fetch(
    `${backendUrl}/sightings/${sightingId}/confirmation`,
    {
      method: "PATCH",
      headers: {
        authorization: `Basic ${btoa(`${username}:${password}`)}`,
        "content-type": "application/json",
      },
      body: JSON.stringify({ newConfirmationStatus: confirmationCode }),
    }
  );
  if (!response.ok) {
    if (response.status === 404) {
      throw new Error("Couldn't connect to database");
    }
    if (response.status === 401) {
      throw new Error(
        "Unauthorised - please check your login info and try again"
      );
    }
    if (response.status === 400) {
      throw new Error("Bad request - please contact Nick & Leo");
    }
  }
};

export const getSightings = async (
  query: URLSearchParams
): Promise<Sighting[]> => {
  const response = await fetch(
    `${backendUrl}/sightings${query.toString() ? `?${query.toString()}` : ""}`
  );
  const sightingsListResponse: ListResponse<Sighting> = await response.json();
  console.log(sightingsListResponse.items);
  return sightingsListResponse.items;
};

export const getSightingsBySpeciesId = async (
  speciesId: string
): Promise<Sighting[]> => {
  const response = await fetch(`${backendUrl}/sightings/species/${speciesId}`);
  const sightingsListResponse: ListResponse<Sighting> = await response.json();
  return sightingsListResponse.items;
};

export const getSightingsByLocationId = async (
  locationId: string
): Promise<Sighting[]> => {
  const response = await fetch(
    `${backendUrl}/sightings/locations/${locationId}`
  );
  const sightingsListResponse: ListResponse<Sighting> = await response.json();
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

export async function createSighting(
  createSightingRequest: CreateSightingRequest
): Promise<boolean> {
  const response = await fetch(`${backendUrl}/sightings`, {
    method: "POST",
    body: JSON.stringify(createSightingRequest),
    headers: { "Content-Type": "application/json" },
  });
  return response.ok;
}

export const getExternalSightings = async (): Promise<ExternalSighting[]> => {
  //the site depended on an external API - I've disabled it to show how the internal site worked

  // const response = await fetch(
  //   `https://whale-spotting-external-api.herokuapp.com/api/sightings`
  // );
  // const listResponse: SightingListResponse = await response.json();
  const listResponse: SightingListResponse = { sightings: [] };
  return listResponse.sightings;
};

export const createUser = async (
  username: string,
  password: string,
  user: CreateUserRequest
): Promise<boolean> => {
  const response = await fetch(`${backendUrl}/users/create`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Basic ${btoa(`${username}:${password}`)}`,
    },
    body: JSON.stringify(user),
  });

  return response.ok;
};
