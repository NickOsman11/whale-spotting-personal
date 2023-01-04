import {
  ExternalSighting,
  GenericSighting,
  isExternalSighting,
  Sighting,
} from "../../clients/apiClient";
import GoogleMapReact from "google-map-react";
import { Marker } from "./Marker";
import React from "react";
import "./SightingMap.scss";

interface SightingMapProps {
  sightings: GenericSighting[];
}

export const SightingMap: React.FC<SightingMapProps> = ({ sightings }) => {
  const points = sightings.map((sighting) => {
    const id = isExternalSighting(sighting)
      ? `ext-${sighting.id}`
      : sighting.id;
    const title =
      (sighting as Sighting).seenBy ??
      (sighting as ExternalSighting).email ??
      "Anonymous";
    const latitude =
      (sighting as Sighting).latitude ??
      (sighting as ExternalSighting).location.latitude;
    const longitude =
      (sighting as Sighting).longitude ??
      (sighting as ExternalSighting).location.longitude;

    let speciesName: string | undefined;
    if (isExternalSighting(sighting)) {
      speciesName = sighting.species.map((s) => s.name).join(", ");
    } else {
      speciesName = sighting.species?.name;
    }

    return {
      id: id,
      title: title,
      speciesName: speciesName,
      lat: latitude,
      lng: longitude,
    };
  });

  const apiKey = process.env.REACT_APP_MAP_API_KEY;
  if (apiKey === undefined) {
    throw new Error(
      "Environment variable REACT_APP_MAP_API_KEY is not defined"
    );
  }

  return (
    <div className="map-container">
      <GoogleMapReact
        bootstrapURLKeys={{
          key: apiKey,
          language: "en",
          region: "GB",
        }}
        defaultCenter={{ lat: 51.506, lng: -0.169 }}
        defaultZoom={5}
      >
        {points.map(({ lat, lng, id, speciesName, title }) => {
          return (
            <Marker
              title={title}
              speciesName={speciesName}
              key={id}
              lat={lat}
              lng={lng}
            />
          );
        })}
      </GoogleMapReact>
    </div>
  );
};
