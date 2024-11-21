export default class ProfileFormValues {
  displayName: string = ''
  bio?: string

  constructor(other?: ProfileFormValues) {
    if (other) {
      this.displayName = other.displayName
      this.bio = other.bio
    }
  }
}
