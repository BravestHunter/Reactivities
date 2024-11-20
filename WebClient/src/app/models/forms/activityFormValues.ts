export default class ActivityFormValues {
  id?: number = undefined
  title: string = ''
  date: Date | null = null
  description?: string = undefined
  category: string = ''
  city: string = ''
  venue: string = ''
  isCancelled: boolean = false

  constructor(other?: ActivityFormValues) {
    if (other) {
      this.id = other.id
      this.title = other.title
      this.date = other.date
      this.description = other.description
      this.category = other.category
      this.city = other.city
      this.venue = other.venue
      this.isCancelled = other.isCancelled
    }
  }
}
