import Photo from './photo'
import User from './user'

export interface Profile {
  username: string
  displayName: string
  bio?: string
  profilePhotoUrl?: string
  following: boolean
  followersCount: number
  followingCount: number
  photos: Photo[]
}

export class Profile implements Profile {
  username: string
  displayName: string
  bio?: string
  profilePhotoUrl?: string

  constructor(user: User) {
    this.username = user.username
    this.displayName = user.displayName
    this.profilePhotoUrl = user.profilePhotoUrl
  }
}
