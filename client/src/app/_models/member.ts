import { Photo } from "./photo";
//We just created an interface for member -- any member created shoudl include these properties
//the interface helps inforce ruling...
//we created this interface by converting the Json from the api to TypeScript
export interface Member {
    id: number;
    username: string;
    photoUrl: string;
    age: number;
    knownAs: string;
    created: Date;
    lastActive: Date;
    gender: string;
    introduction: string;
    lookingFor: string;
    interests: string;
    city: string;
    country: string;
    photos: Photo[];
}


