import { Grid, Loader } from 'semantic-ui-react'
import ActivityList from './ActivityList'
import { useMainStore } from '../../../app/stores/mainStore'
import { observer } from 'mobx-react-lite'
import { useEffect, useState } from 'react'
import ActivityFilters from './ActivityFilters'
import InfiniteScroll from 'react-infinite-scroller'
import ActivityListItemPlaceholder from './ActivityListItemPlaceholder'

export default observer(function ActivityDashboard() {
  const { activityStore } = useMainStore()
  const { loadActivities, activityRegistry, currentPage, hasMore } =
    activityStore
  const [loadingNext, setLoadingNext] = useState(false)

  function handleGetNext() {
    setLoadingNext(true)
    loadActivities(currentPage + 1).then(() => setLoadingNext(false))
  }

  useEffect(() => {
    if (activityRegistry.size <= 1) loadActivities(1)
  }, [activityRegistry.size, loadActivities])

  return (
    <Grid>
      <Grid.Column width="10">
        {activityStore.loadingInitial && !loadingNext ? (
          <>
            <ActivityListItemPlaceholder />
            <ActivityListItemPlaceholder />
          </>
        ) : (
          <InfiniteScroll
            pageStart={0}
            loadMore={handleGetNext}
            hasMore={!loadingNext && hasMore}
            initialLoad={false}
          >
            <ActivityList />
          </InfiniteScroll>
        )}
      </Grid.Column>
      <Grid.Column width="6">
        <ActivityFilters />
      </Grid.Column>
      <Grid.Column width={10}>
        <Loader active={loadingNext} />
      </Grid.Column>
    </Grid>
  )
})
