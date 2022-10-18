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

export interface PendingSighting {
  id: number;
  species?: string;
  seenBy: string;
  seenOn: Date;
  imageUrl?: string;
  description?: string;
  whaleCount: number;
  location?: string;
  latitude: number;
  longitude: number;
}

export interface ConfirmationRequest {
  SightingId: number;
  ConfirmationStatus: number;
}

export interface ConfirmationRequestResponse {
  SightingId: number;
  ConfirmationStatus: number;
  StatusCode: number;
}

export const getAllSpecies = async (): Promise<Species[]> => {
  const response = await fetch(`${backendUrl}/whales`);
  const whaleListResponse: ListResponse<Species> = await response.json();
  return whaleListResponse.items;
};

export const getAllPendingSightings = async (): Promise<PendingSighting[]> => {
  const response = await fetch(`${backendUrl}/sightings/pending`);
  const pengingSighting: ListResponse<PendingSighting> = await response.json();
  return pengingSighting.items;
};

// export const confirmOrRejectSighting = async (
//   confirmationRequest: ConfirmationRequest,
//   username: string,
//   password: string,
//   setStatus: React.Dispatch<React.SetStateAction<string>>
// ): Promise<boolean> => {
//   const response = await fetch(`${backendUrl}/sightings/pending`, {
//     method: "PATCH",
//     headers: {
//       authorization: `Basic ${btoa(`${username}:${password}`)}`,
//     },
//     body: JSON.stringify(confirmationRequest),
//   });

//   if (!response.ok) {
//     setStatus("Error. Please try again");
//     return false;
//   } else {
//     if(confirmationRequest.ConfirmationStatus === 2){
//       setStatus("Approved")
//     } else if(confirmationRequest.ConfirmationStatus === 1){
//       setStatus("Rejected")
//     }
//     return true;
//   }
// };

export const confirmOrRejectSighting = (
  confirmationRequest: ConfirmationRequest,
  username: string,
  password: string
): boolean => {
  // const response = await fetch(`${backendUrl}/sightings/pending`, {
  //   method: "PATCH",
  //   headers: {
  //     authorization: `Basic ${btoa(`${username}:${password}`)}`,
  //   },
  //   body: JSON.stringify(confirmationRequest),
  // });

  // if (!response.ok) {
  //   setStatus("Error. Please try again");
  //   return false;
  // } else {
  console.log("Print Request", confirmationRequest);
  return true;
  // }
};
