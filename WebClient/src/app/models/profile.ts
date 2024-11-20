import Photo from './photo'
import { ProfileShort } from './profileShort'

export interface Profile extends ProfileShort {
  username: string
  displayName: string
  bio?: string
  profilePhotoUrl?: string
  following: boolean
  followersCount: number
  followingCount: number
  photos: Photo[]
}
