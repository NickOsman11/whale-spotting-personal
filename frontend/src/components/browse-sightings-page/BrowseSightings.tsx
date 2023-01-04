import React, { useEffect, useState } from "react";
import {
  getSightings,
  getSightingsBySpeciesId,
  getExternalSightings,
  Sighting,
  ExternalSighting,
  GenericSighting,
  getDate,
} from "../../clients/apiClient";
import { SightingMap } from "../sighting-map/SightingMap";
import { SightingList } from "../sighting-list/SightingList";
import "./BrowseSightings.scss";
import { useParams } from "react-router-dom";
import { compareDesc } from "date-fns";

export const BrowseSightings: React.FC = () => {
  const [sightings, setSightings] = useState<Sighting[]>();
  const [externalSightings, setExternalSightings] =
    useState<ExternalSighting[]>();

  let allSightings: GenericSighting[] = (
    (sightings as GenericSighting[]) ?? []
  ).concat(...(externalSightings ?? []));

  allSightings = allSightings.sort((a, b) =>
    compareDesc(getDate(a), getDate(b))
  );

  const [isShowingMap, setIsShowingMap] = useState(false);

  const { speciesId } = useParams<{ speciesId: string }>();

  useEffect(() => {
    if (speciesId === undefined) {
      getSightings().then((sightings) => setSightings(sightings));
      getExternalSightings().then(setExternalSightings);
    } else {
      getSightingsBySpeciesId(speciesId).then(setSightings);
    }
  }, [speciesId]);

  let contents = <></>;

  if (sightings === undefined && externalSightings === undefined) {
    contents = <p>Loading</p>;
  } else if (
    sightings !== undefined &&
    sightings.length === 0 &&
    speciesId !== undefined
  ) {
    contents = <p>Sorry, we have no sightings of that species!</p>;
  } else if (
    sightings !== undefined &&
    externalSightings !== undefined &&
    allSightings.length === 0
  ) {
    contents = <p>Sorry, there are no sightings to display.</p>;
  } else if (
    sightings !== undefined &&
    externalSightings === undefined &&
    allSightings.length === 0
  ) {
    contents = (
      <>
        <p>Sorry, there are no sightings to display.</p>
        <p>Loading sightings from the Washington Whale Hotline...</p>
      </>
    );
  } else {
    contents = (
      <>
        <button
          className="map-button"
          onClick={() => {
            setIsShowingMap(!isShowingMap);
          }}
        >
          {isShowingMap ? "List View" : "Map View"}
        </button>
        {isShowingMap ? (
          <SightingMap sightings={allSightings} />
        ) : (
          <SightingList sightings={allSightings} />
        )}
      </>
    );
  }

  return (
    <>
      <h1>Reported Sightings</h1>
      {contents}
    </>
  );
};
