"use client";
import React, { useMemo, useEffect } from "react";
import { trpc } from "@/app/_trpc/client";
import { useShallow } from "zustand/react/shallow";
import { useParamsStore } from "@/app/_hooks/useParamsStore";
import { useAuctionStore } from "@/app/_hooks/useAuctionStore";

import AuctionCard from "@/app/_components/AuctionCard/AuctionCard";
import Pagination from "@/app/_components/Pagination/Pagniation";
import PageSize from "@/app/_components/PageSize/PageSize";
import Filter from "@/app/_components/Filters/Filter";
import EmptyList from "@/app/_components/EmptyList/EmptyList";

import type { PagedAuction } from "@/server/schemas/auction";

type Props = {
  initialAuctions: PagedAuction | undefined;
};

const AuctionList = ({ initialAuctions }: Props) => {
  const params = useParamsStore(
    useShallow((state) => ({
      pageNumber: state.pageNumber,
      pageSize: state.pageSize,
      searchTerm: state.searchTerm,
      filterBy: state.filterBy,
      orderBy: state.orderBy,
      seller: state.seller,
      winner: state.winner,
    }))
  );
  const setParams = useParamsStore((state) => state.setParams);

  const { auctions, pageCount } = useAuctionStore(
    useShallow((state) => ({
      auctions: state.auctions,
      totalCount: state.totalCount,
      pageCount: state.pageCount,
    }))
  );
  const setAuctions = useAuctionStore((state) => state.setData);

  const { data, isLoading, isError } = trpc.auctions.list.useQuery(
    {
      pageNumber: params.pageNumber,
      pageSize: params.pageSize,
      searchTerm: params.searchTerm,
      filterBy: params.filterBy,
      orderBy: params.orderBy,
      seller: params.seller,
      winner: params.winner,
    },
    {
      initialData: initialAuctions,
      refetchOnMount: false,
      refetchOnReconnect: false,
      refetchOnWindowFocus: false,
    }
  );

  useEffect(() => {
    if (!isLoading && !isError) {
      setAuctions(data);
    }
  }, [data, isLoading, isError]);

  return (
    <div className="w-full flex flex-col lg:flex-row">
      <Filter />
      {isLoading && <div>Loading...</div>}
      {!isLoading && !isError && !data.totalCount && <EmptyList showReset />}
      {!isLoading && !isError && data.totalCount > 0 && (
        <div>
          <div className="grid grid-cols-1 gap-6 auto-rows-max  lg:grid-cols-3">
            {auctions.map((auction, idx) => (
              <div key={idx} className="w-full">
                <AuctionCard auction={auction} />
              </div>
            ))}
          </div>
          <div className="w-full flex justify-between my-2">
            <div></div>
            <Pagination
              currentPage={params.pageNumber}
              onPageChange={(pageNumber) => setParams({ pageNumber })}
              totalPages={pageCount}
            />
            <PageSize
              pageSize={params.pageSize}
              onPageSizeChange={(newPageSize) =>
                setParams({ pageSize: newPageSize })
              }
            />
          </div>
        </div>
      )}
    </div>
  );
};

AuctionList.displayName = "AuctionList";
export default AuctionList;
